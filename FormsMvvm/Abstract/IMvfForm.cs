﻿using System.Windows.Forms;

namespace FormsMvvm.Abstract
{
    public interface IMvfForm
    {
        Control.ControlCollection Controls { get; set; }
    }
}