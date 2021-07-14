using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Frontend_PI.Models
{
    public class LockUnlockUser
    {


    


        public LockUnlockUser(string label, double y)
        {
            this.label = label;
            this.y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public double y = 0;


    }
}