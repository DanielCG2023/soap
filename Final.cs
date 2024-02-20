using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using System.ServiceModel;

public class Form1 : Form
{
  public Form3()
  {
      InitializeComponent();
  }
  private async void button1_Click(object sender, EventArgs e)
  {
      X509Certificate2 clientCertificate = new X509Certificate2("C:\\Users\\Daniel\\Desktop\\certificado\\SERCOLUX.p12", "Arriaga01013");
      var b = new CustomBinding();
      
      var sec = (AsymmetricSecurityBindingElement)SecurityBindingElement.CreateMutualCertificateBindingElement(MessageSecurityVersion.WSSecurity10WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10);
      sec.EndpointSupportingTokenParameters.Signed.Add(new X509SecurityTokenParameters());
      sec.MessageSecurityVersion=  MessageSecurityVersion.WSSecurity10WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10;
      sec.IncludeTimestamp = false;
      sec.MessageProtectionOrder = System.ServiceModel.Security.MessageProtectionOrder.EncryptBeforeSign;
  
  
      b.Elements.Add(sec);
      b.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8));
      b.Elements.Add(new HttpsTransportBindingElement());
  
  
      var c = new VTCPortClient(b, new EndpointAddress(new Uri("https://sede.mitma.gob.es/MFOM.Services.VTC.Server/VTCPort"), new DnsEndpointIdentity("16056013N AGUSTIN GUTIERREZ (R: B01151968)"), new AddressHeaderCollection()));
  
      c.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode =
          System.ServiceModel.Security.X509CertificateValidationMode.None;
      c.ClientCredentials.ServiceCertificate.DefaultCertificate = new X509Certificate2("C:\\Users\\Daniel\\Desktop\\certificado\\SERCOLUX.p12", "Arriaga01013");
  
      c.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2("C:\\Users\\Daniel\\Desktop\\certificado\\SERCOLUX.p12", "Arriaga01013");
  
      c.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;
  
      qconsultavtc qConsultaVTC = new qconsultavtc();
      HEADERTYPE header = new HEADERTYPE();
      CBODYTYPE body = new CBODYTYPE();
      qConsultaVTC.header = header;
      qConsultaVTC.body = body;
      DateTime fecha = DateTime.Now;
      qConsultaVTC.header.fecha = fecha;
      qConsultaVTC.header.version = "1.0";
      qConsultaVTC.header.versionsender = "1.0";
      EVTCCONSULTA vtcconsulta = new EVTCCONSULTA();
      qConsultaVTC.body.vtcconsulta = vtcconsulta;
      qConsultaVTC.body.vtcconsulta.idservicio = 202952766;
  
      try
      {
          var resp = await c.ConsultaDeServicioAsync(qConsultaVTC);
      }catch(Exception ex)
      {
          MessageBox.Show(""+ex.Message);
      } 
    }
}
