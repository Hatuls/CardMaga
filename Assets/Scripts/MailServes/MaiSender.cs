using Account;
using UnityEngine;

namespace MailServes
{
    [CreateAssetMenu (fileName = "emailSender", menuName = "ScriptableObjects/Email/Email Sender")]
    public class MaiSender : ScriptableObject
    {
        [SerializeField] private string _emailAddres;
        public void SendEmail()
        {
            string email = _emailAddres;
            string subject = MyEscapeURL($"Contact Team DEVinci UserID: {AccountManager.Instance.PlayfabID}");
            string body = MyEscapeURL("My Body\r\nFull of non-escaped chars");
            Application.OpenURL ("mailto:" + email + "?subject=" + subject + "&amp;body=" + body);
        }

        string MyEscapeURL (string URL)
        {
            return WWW.EscapeURL(URL).Replace("+","%20");
        }
    }
}