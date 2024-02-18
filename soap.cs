 public async Task ejecutarAsync()
 {
     servicio.VTCPortClient client;

     try
     {
         
         string logFilePath = @"C:\Log\log.txt";
         Trace.Listeners.Add(new TextWriterTraceListener(logFilePath));
         Trace.AutoFlush = true;


         X509Certificate2 cert = new X509Certificate2("C:\\Users\\Daniel\\Desktop\\certificado\\test.p12", "A01013");
         X509Certificate2 signingCert = cert;
         var bindings = new BasicHttpsBinding();

         bindings.Security.Mode = BasicHttpsSecurityMode.TransportWithMessageCredential;
         bindings.Security.Transport.ClientCredentialType = HttpClientCredentialType.Digest;
         bindings.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
         bindings.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Basic256Sha256;



         var elements = bindings.CreateBindingElements();
         var securityBindingElement = elements.Find<AsymmetricSecurityBindingElement>();
         if (securityBindingElement != null)
         {
             securityBindingElement.MessageProtectionOrder = MessageProtectionOrder.SignBeforeEncrypt;
         }
         elements.Find<SecurityBindingElement>().EnableUnsecuredResponse = true;
         var customBindings = new CustomBinding(elements);
         client = new VTCPortClient(
             customBindings,
             new EndpointAddress(@"https://sede.mitma.gob.es/MFOM.Services.VTC.Server/VTCPort"));

         foreach (OperationDescription op in client.Endpoint.Contract.Operations)
         {
             var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();
             if (dataContractBehavior != null)
             {
                 dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
             }
         }
         client.ClientCredentials.ClientCertificate.Certificate = signingCert;
         client.ClientCredentials.ServiceCertificate.DefaultCertificate = signingCert;

         client.Open();

         ConsultaDeServicio cds = new ConsultaDeServicio();
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
         qConsultaVTC.body.vtcconsulta.idservicio = 1;
         cds.qconsultavtc = qConsultaVTC;
         var request = qConsultaVTC;
         //SignMessage(GetType(), request);

         var response = await client.ConsultaDeServicioAsync(qConsultaVTC);


         Console.WriteLine(response);
         Trace.Close();
     }
     catch (Exception ex_)
     {

         Trace.Close();       
     }
 }
