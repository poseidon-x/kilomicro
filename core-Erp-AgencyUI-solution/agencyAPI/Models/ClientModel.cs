using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using coreLogic;

namespace agencyAPI.Models
{
    // Models used as parameters to AccountController actions.

    public class ClientModel 
    {

        public client client { get; set; }
        public address clientAddress { get; set; }

        public phone clientPhone { get; set; }

        public email clientEmail { get; set; }

    }

}
