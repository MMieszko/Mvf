﻿namespace FormsMvvm.Abstract
{
    public interface IMvfForm
    {
        void OnLoad();
        void OnClosed();
        void OnViewModelSet();
    }
}
