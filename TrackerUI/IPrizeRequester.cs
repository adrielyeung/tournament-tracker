using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public interface IPrizeRequester
    {
        void PrizeComplete(PrizeModel model);
    }
}
