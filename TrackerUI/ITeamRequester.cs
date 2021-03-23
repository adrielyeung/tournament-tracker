using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public interface ITeamRequester
    {
        void TeamComplete(TeamModel model);
    }
}
