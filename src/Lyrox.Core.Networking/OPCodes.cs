namespace Lyrox.Core.Networking
{
    public enum OPCode
    {
        //Handshake
        //SB
        Handshake = 0x00,
        //Login
        //CB
        DisconnectLogin = 0x00,
        EncryptionRequest = 0x01,
        LoginSucess = 0x02,
        SetCompression = 0x03,
        LoginPluginRequest = 0x04,
        //SB
        LoginStart = 0x00,
        EncryptionResponse = 0x01,
        LoginPluginResponse = 0x02,
        //Play
        //CB
    }
}
