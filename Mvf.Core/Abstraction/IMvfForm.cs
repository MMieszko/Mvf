namespace Mvf.Core.Abstraction
{
    public interface IMvfForm
    {
        void OnLoad();
        void OnClosed();
        void OnViewModelSet();
    }
}
