﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms; 
using coreLogic;

namespace coreBusiness
{
    public interface IClientManager  
    {
        void SaveClientData(client cl);
    }
}
