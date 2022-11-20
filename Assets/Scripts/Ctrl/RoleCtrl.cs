using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using FSM;
using FSM.AI;
using FSM.RoleInfo;
using UnityEngine;

namespace Ctrl
{
    [RequireComponent(typeof(Animator))]
    public class RoleCtrl : MonoBehaviour
    {
        #region 成员变量
        public Animator Animator;

        private Vector3 m_TargetPos = Vector3.zero;

        private CharacterController m_CharacterController;

        [SerializeField]
        private float m_Speed;

        [SerializeField]
        private float m_RotSpeed;

        private bool m_IsRotingOver;
        private Quaternion m_TargetQuaternion;

        private CancellationTokenSource source;

        /// <summary>
        /// 当前角色类型
        /// </summary>
        public ERoleType CurrRoleType = ERoleType.None;

        /// <summary>
        /// 当前角色信息
        /// </summary>
        public RoleInfoBase CurrRoleInfo = null;

        /// <summary>
        ///  当前角色AI
        /// </summary>
        public IRoleAI CurrRoleAI = null;

        /// <summary>
        /// 当前角色有限状态机管理器
        /// </summary>
        public RoleFSMMgr CurrRoleFSMMgr = null;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="roleType">角色类型</param>
        /// <param name="roleInfo">角色信息</param>
        /// <param name="ai">角色AI</param>
        public void Init(ERoleType roleType, RoleInfoBase roleInfo, IRoleAI ai)
        {
            CurrRoleType = roleType;
            CurrRoleInfo = roleInfo;
            CurrRoleAI = ai;
        }

        void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
            if (!m_CharacterController.isGrounded)
            {
                m_CharacterController.Move(
                    (transform.position + new Vector3(0, -1000, 0)) - transform.position
                );
            }

            CurrRoleFSMMgr = new RoleFSMMgr(this);
        }

        private void Reset()
        {
            Animator.SetBool("ToIdle", false);
            Animator.SetBool("ToWalk", false);
            Animator.SetBool("ToRun", false);
            Animator.SetBool("ToDie", false);
            Animator.SetBool("ToJump", false);
            Animator.SetBool("ToHurt", false);
            Animator.SetInteger("ToPyhAttack", 0);
        }

        /// <summary>
        /// 移动角色
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private async UniTaskVoid Movement(Vector3 target)
        {
            Reset();
            Animator.SetBool("ToWalk", true);
            while (Vector3.Distance(m_TargetPos, transform.position) > 0.27f)
            {
                //让角色插值移动
                Vector3 direction = m_TargetPos - transform.position;
                direction = direction.normalized;
                direction = direction * Time.deltaTime * m_Speed;
                direction.y = 0;
                m_CharacterController.Move(direction);

                //让角色缓慢转身
                if (!m_IsRotingOver)
                {
                    // transform.LookAt(new Vector3(m_TargetPos.x, transform.position.y, m_TargetPos.z));
                    m_RotSpeed += 0.2f;
                    m_TargetQuaternion = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Lerp(
                        transform.rotation,
                        m_TargetQuaternion,
                        Time.deltaTime * m_RotSpeed
                    );

                    if (Quaternion.Angle(transform.rotation, m_TargetQuaternion) < 1.0f)
                    {
                        m_RotSpeed = 0.2f;
                        m_IsRotingOver = true;
                    }
                }
                await UniTask.Yield(source.Token);
            }
            source?.Cancel();

            Reset();
            Animator.SetBool("ToIdle", true);
        }

        /// <summary>
        /// 点击地面
        /// </summary>
        private void OnClickGround()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (
                    hitInfo.collider.gameObject.name.Equals(
                        "Plane",
                        System.StringComparison.CurrentCultureIgnoreCase
                    )
                )
                {
                    m_TargetPos = hitInfo.point;
                    m_IsRotingOver = false;
                    m_RotSpeed = 0.0f;
                }
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        private void UpdateState()
        {
            AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);
            Animator.speed = 1.0f;

            if (info.IsName("Idle01"))
            {
                Animator.SetInteger("CurrentState", (int)ERoleState.Idle01);
            }
            if (info.IsName("Attack01"))
            {
                Animator.SetInteger("CurrentState", (int)ERoleState.Attack);
                Animator.SetInteger("ToPyhAttack", 1);
                if (info.normalizedTime >= 1)
                {
                    Reset();
                    Animator.SetBool("ToIdle", true);
                }
            }
            if (info.IsName("Attack02"))
            {
                Animator.SetInteger("CurrentState", (int)ERoleState.Attack);
                Animator.SetInteger("ToPyhAttack", 2);
                if (info.normalizedTime >= 1)
                {
                    Reset();
                    Animator.SetBool("ToIdle", true);
                }
            }
            if (info.IsName("Run01"))
            {
                Animator.SetInteger("CurrentState", (int)ERoleState.Run01);
            }
            if (info.IsName("Walk01"))
            {
                Animator.SetInteger("CurrentState", (int)ERoleState.Walk01);
            }
            if (info.IsName("Dead01"))
            {
                Animator.SetInteger("CurrentState", (int)ERoleState.Dead01);
            }
            if (info.IsName("Hurt01"))
            {
                Animator.SetInteger("CurrentState", (int)ERoleState.Hurt01);
            }
            if (info.IsName("Jump01"))
            {
                Animator.SetInteger("CurrentState", (int)ERoleState.Jump01);
                Animator.speed *= 1.5f;
                if (info.normalizedTime >= 1)
                {
                    Reset();
                    Animator.SetBool("ToIdle", true);
                }
            }
        }

        /// <summary>
        /// 键盘监控
        /// </summary>
        private void KeyCtrl()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Reset();
                Animator.SetBool("ToJump", true);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Reset();
                Animator.SetBool("ToRun", true);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Reset();
                Animator.SetInteger("ToPyhAttack", 1);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Reset();
                Animator.SetInteger("ToPyhAttack", 2);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                Reset();
                Animator.SetBool("ToIdle", true);
            }
        }

        void Update()
        {
            //没有AI直接返回
            if (CurrRoleAI == null)
                return;
            CurrRoleAI.DoAI();

            if (CurrRoleFSMMgr != null)
                CurrRoleFSMMgr.OnUpdate();

            if (m_CharacterController == null)
                return;

            UpdateState();
            KeyCtrl();

            if (Input.GetMouseButtonUp(0) || Input.touchCount == 1)
            {
                OnClickGround();
                //协程控制更新移动
                if (m_TargetPos != Vector3.zero)
                {
                    source?.Cancel();
                    source = new CancellationTokenSource();
                    Movement(m_TargetPos).Forget();
                }
            }
        }

        private void OnDestroy()
        {
            source?.Cancel();
            source?.Dispose();
        }
    }
}
