////*************************Copyright © 2008 Feng 豐**************************	
// Created    : 12/22/2011 8:54:16 AM 
// Description: Engine.cs  
// Revisions  :            		
// **************************************************************************** 
using System;
using System.Collections.Generic;
using System.Text;
using Common.DataCore;

namespace Demo
{
    public class BossJobEngine
    {
        public BossJobEngine(IEntityTableSource policySource){
            policySource.TryGetTable(out teamTblProxy, "Demo", "Team", "*");
            policySource.TryGetTable(out jobTblProxy, "Demo", "BossJob", "*");
            policySource.TryGetTable(out codeTblProxy, "Demo", "CodeMessage", "*");            
        }
        public void Exec(DateTime tm){       
            var api = ApiFactory<IBossJobApi>.Create(
                new SysAction() {Codes=codeTblProxy,CurrentDate=tm}
                , new TeamAction()
             );
            foreach (ITeam team in teamTblProxy)
            {
                foreach (IMember member in team.Members)
                {                    
                    api.Team.Member = member;
                    api.Team.Team = team;                   
                    foreach (IBossJob job in jobTblProxy)                    
                        if (job.Cond.Exec(api))   job.ToDo.Exec(api);                    
                }
            }
        }
        private EntityTableProxy<IBossJob> jobTblProxy;
        private EntityTableProxy<ITeam> teamTblProxy;
        private EntityTableProxy<ICodeMessage> codeTblProxy;        
    }
}
