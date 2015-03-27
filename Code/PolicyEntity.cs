////*************************Copyright © 2010 PolicyBuilder**************************	
// Created    : 2011/12/22 上午 09:03:36 
// Description: ICodeMessage.cs
// Revisions  :            		
// **************************************************************************** 
using System;
using System.Collections.Generic;
using System.Text;
using Common.DataCore;

namespace Demo
{
    public interface ICodeMessage : IDictionaryAccess
    {
        [EntityProperty(IsKey = true, KeyGen = "id_{0}")]
        string Id { get; set; }
        string Message { get; set; }
    }

    public interface IBossJob : IDictionaryAccess
    {
        [EntityProperty(IsKey = true, KeyGen = "id_{0}")]
        string Id { get; set; }
        FunctionField<bool> Cond { get; }
        ActionField ToDo { get; }
        Boolean IsEnable { get; set; }
        string Description { get; set; }
    }

    public interface ITeam : IDictionaryAccess
    {
        [EntityProperty(IsKey = true, KeyGen = "id_{0}")]
        string Id { get; set; }
        string TeamId { get; set; }
        ForeignSetField<IMember> Leader { get; }
        ForeignSetField<IMember> Members { get; }
        string Description { get; set; }
    }

    public interface IMember : IDictionaryAccess
    {
        [EntityProperty(IsKey = true, KeyGen = "id_{0}")]
        string Id { get; set; }
        string Name { get; set; }
        DateTime BirthDate { get; set; }
        string Tel { get; set; }
        string EMail { get; set; }
        string Group { get; set; }
        DateTime PromotAt { get; set; }
    }

}