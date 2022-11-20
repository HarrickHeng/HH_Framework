
//===================================================
//创建时间：2022-11-11 08:12:21
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;
using HHFramework;

/// <summary>
/// Chapter数据管理
/// </summary>
public partial class ChapterDBModel : DataTableDBModelBase<ChapterDBModel, ChapterEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    public override string DataTableName => "Chapter";

    /// <summary>
    /// 加载列表
    /// </summary>
    protected override void LoadList(MMO_MemoryStream ms)
    {
        var rows = ms.ReadInt();
        var columns = ms.ReadInt();

        for (var i = 0; i < rows; i++)
        {
            var entity = new ChapterEntity
            {
                Id = ms.ReadInt(),
                ChapterName = ms.ReadUTF8String(),
                GameLevelCount = ms.ReadInt(),
                Big_Pic = ms.ReadUTF8String(),
                Uvx = ms.ReadFloat(),
                Uvy = ms.ReadFloat()
            };

            mList.Add(entity);
            mDic[entity.Id] = entity;
        }
    }
}