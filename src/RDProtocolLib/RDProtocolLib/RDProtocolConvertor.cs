using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace RDProtocolLib
{
    public class RDProtocolConvertor
    {
        public static void RDMessageToByteArray(RDProtocol message, out byte[] byteMessage)
        {
            var formatter = new BinaryFormatter();            
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, message);
                byteMessage = stream.GetBuffer();
            }
        }

        public static void ByteArrayToRDMessage(byte[] byteMessage, out RDProtocol message)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(byteMessage, 0, byteMessage.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                message = (RDProtocol) binForm.Deserialize(memStream);
            }
        }
        public static void ClientKeysToByteArray(ClientKeys clientKeys, out byte[] byteMessage)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, clientKeys);
                byteMessage = stream.GetBuffer();
            }
        }
        public static void ByteArrayToClientKeys(byte[] byteMessage, out ClientKeys clientKeys)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(byteMessage, 0, byteMessage.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                clientKeys = (ClientKeys) binForm.Deserialize(memStream);
            }
        }
        public static void MouseCoordinatesToByteArray(MouseCoordinates mouseCoordinates, out byte[] byteMessage)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, mouseCoordinates);
                byteMessage = stream.GetBuffer();
            }
        }
        public static void ByteArrayToMouseCoordinates(byte[] byteMessage, out MouseCoordinates mouseCoordinates)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(byteMessage, 0, byteMessage.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                mouseCoordinates = (MouseCoordinates) binForm.Deserialize(memStream);
            }
        }

        public static void RDEndpointToByteArray(RDEndpoint rdEndpoint, out byte[] byteMessage)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, rdEndpoint);
                byteMessage = stream.GetBuffer();
            }
        }
        public static void ByteArrayToRDEndpoint(byte[] byteMessage, out RDEndpoint rdEndpoint)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(byteMessage, 0, byteMessage.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                rdEndpoint = (RDEndpoint) binForm.Deserialize(memStream);
            }
        }
        public static void ClientsInfoToByteArray(ClientsInfo clientsInfo, out byte[] byteMessage)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, clientsInfo);
                byteMessage = stream.GetBuffer();
            }
        }
        public static void ByteArrayToClientsInfo(byte[] byteMessage, out ClientsInfo clientsInfo)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(byteMessage, 0, byteMessage.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                clientsInfo = (ClientsInfo)binForm.Deserialize(memStream);
            }
        }
        public static void ClientLoginDataToByteArray(ClientLoginData clientData, out byte[] byteMessage)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, clientData);
                byteMessage = stream.GetBuffer();
            }
        }
        public static void ByteArrayToClientLoginData(byte[] byteMessage, out ClientLoginData clientData)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(byteMessage, 0, byteMessage.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                clientData = (ClientLoginData)binForm.Deserialize(memStream);
            }
        }
    }
}
