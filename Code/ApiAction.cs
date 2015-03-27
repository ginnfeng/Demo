////*************************Copyright © 2008 Feng 豐**************************	
// Created    : 12/22/2011 8:54:01 AM 
// Description: ApiAction.cs  
// Revisions  :            		
// **************************************************************************** 
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Common.DataCore;

namespace Demo
{
    public interface IBossJobApi
    {
        SysAction Sys { get; set; }
        TeamAction Team { get; set; }
    }

    public class SysAction
    {
        public SysAction() {}
        public int ThanYears(DateTime date)
        {
            TimeSpan tmSpan = CurrentDate - date;
            return tmSpan.Days / 365;
        }
        public int ThanDays(DateTime date)
        {
            return Math.Abs(CurrentDate.Day - date.Day);            
        }
        public void SendMail(string code, IMember to, IMember who)
        {
            //IMember who = Team.Members.Find(m => m.Id == memberId);
            var msgCode = Codes.Find(it => it.Id == code);
            var msg = string.Format(msgCode.Message, who.Name);
            MessageBox.Show(msg, to.Name+":"+to.EMail, MessageBoxButtons.OK);
        }
        public DateTime CurrentDate { get; set; }
        public EntityTableProxy<ICodeMessage> Codes { get; set; }
    }

    public class TeamAction
    {
        public TeamAction(){}
        public ITeam Team { get; set; }
        public IMember Leader { get { return Team.Leader.First; } }
        public IMember Member { get; set; }        
        
    }
    
}
