using xServer.KuuhakuCekirdek.MouseKeyHook.Uygulama;

namespace xServer.KuuhakuCekirdek.MouseKeyHook
{
    public static class AnaHook
    {
        public static KlavyeFareEylemleri AppEvents()
        {
            return new AppEventFacade();
        }

        public static KlavyeFareEylemleri GlobalEvents()
        {
            return new GlobalEventFacade();
        }
    }
}