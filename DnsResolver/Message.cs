using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Network.Dns
{
    public class Message : Message<Message>
    {
        public Message()
        {
            Questions = new List<Question>();
            Answers = new List<Answer>();
            Authorities = new List<Answer>();
            Additionals = new List<Answer>();
        }

        public IPEndPoint From { get; set; }

        public Qr QueryResponse { get; set; }

        public ushort ID { get; set; }

        public OpCode OpCode { get; set; }

        public bool AuthoritativeAnswer { get; set; }

        public bool Truncated { get; set; }

        public bool RecursionDesired { get; set; }

        public bool RecursionAvailable { get; set; }

        public ResponseCode ResponseCode { get; set; }

        public ushort QuestionEntries
        {
            get { return (ushort)Questions.Count; }
        }

        public ushort AnswerEntries
        {
            get { return (ushort)Answers.Count; }
        }

        public ushort AuthorityEntries { get { return (ushort)Authorities.Count; } }

        public ushort AdditionalEntries { get { return (ushort)Additionals.Count; } }

        public IList<Question> Questions { get; set; }
        public IList<Answer> Answers { get; set; }
        public IList<Answer> Authorities { get; set; }
        public IList<Answer> Additionals { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendLine(string.Format("ID : {0}", ID));
            sb.AppendLine(string.Format("Query/Response : {0}", QueryResponse));
            sb.AppendLine(string.Format("OpCode : {0}", OpCode));
            sb.AppendLine(string.Format("Authoritative Answer : {0}", AuthoritativeAnswer));
            sb.AppendLine(string.Format("Truncated : {0}", Truncated));
            sb.AppendLine(string.Format("Recursion Desired : {0}", RecursionDesired));
            sb.AppendLine(string.Format("Recursion Available : {0}", RecursionDesired));
            sb.AppendLine(string.Format("Recursion ResponseCode : {0}", ResponseCode));
            sb.AppendLine(string.Format("Question Entries ({0}) :", QuestionEntries));
            foreach (Question q in Questions)
                sb.AppendLine(q.ToString());
            sb.AppendLine(string.Format("Answer Entries ({0}) :", AnswerEntries));
            foreach (Answer a in Answers)
                sb.AppendLine(a.ToString());

            return sb.ToString();
        }

        #region IResponse Members

        public override void WriteTo(Stream stream)
        {
            //ID
            BinaryHelper.Write(stream, ID);
            //Qr, Opcode, Aa, Tc, Rd
            byte b = 0;
            b += (byte)QueryResponse;
            b = (byte)(b << 4);
            b += (byte)OpCode;
            b = (byte)(b << 1);
            b += (AuthoritativeAnswer) ? (byte)1 : (byte)0;
            b = (byte)(b << 1);
            b += (Truncated) ? (byte)1 : (byte)0;
            b = (byte)(b << 1);
            b += (RecursionDesired) ? (byte)1 : (byte)0;
            stream.WriteByte(b);

            //Ra, Z, Rcode
            b = 0;
            b += (RecursionAvailable) ? (byte)1 : (byte)0;
            b = (byte)(b << 7);
            b += (byte)ResponseCode;
            stream.WriteByte(b);
            BinaryHelper.Write(stream, QuestionEntries);
            BinaryHelper.Write(stream, AnswerEntries);
            BinaryHelper.Write(stream, AuthorityEntries);
            BinaryHelper.Write(stream, AdditionalEntries);
            foreach (Question q in Questions)
                q.WriteTo(stream);
            foreach (Answer a in Answers)
                a.WriteTo(stream);
            foreach (Answer a in Authorities)
                a.WriteTo(stream);
            foreach (Answer a in Additionals)
                a.WriteTo(stream);
        }

        #endregion

        #region IRequest<Message> Members

        protected override Message GetMessage(Stream s)
        {
            return GetMessage(new BackReferenceBinaryReader(s, Encoding.BigEndianUnicode));
        }

        public static Message GetMessage(BinaryReader reader)
        {
            if (!(reader is BackReferenceBinaryReader))
                reader = new BackReferenceBinaryReader(reader.BaseStream, Encoding.BigEndianUnicode);
            Message m = new Message();
            int index = 0;
            ushort id = BinaryHelper.ReadUInt16(reader);
            //FromBytes(bytes, out id);
            m.ID = id;
            index++; index++;
            byte b = reader.ReadByte();
            //byte b = bytes[index++];
            //Qr, Opcode, Aa, Tc, Rd
            m.RecursionDesired = (b % 2) == 1;
            b = (byte)(b >> 1);
            m.Truncated = (b % 2) == 1;
            b = (byte)(b >> 1);
            m.AuthoritativeAnswer = (b % 2) == 1;
            b = (byte)(b >> 1);
            int opCodeNumber = b % 16;
            m.OpCode = (OpCode)opCodeNumber;
            b = (byte)(b >> 4);
            m.QueryResponse = (Qr)b;
            //Ra, Z, Rcode
            b = reader.ReadByte();
            //b = bytes[index++];
            m.RecursionAvailable = b > 127;
            b = (byte)((b << 1) >> 1);
            m.ResponseCode = (ResponseCode)b;
            ushort questionEntryCount, answerEntryCount, authorityEntryCount, additionalEntryCount;
            questionEntryCount = BinaryHelper.ReadUInt16(reader);
            answerEntryCount = BinaryHelper.ReadUInt16(reader);
            authorityEntryCount = BinaryHelper.ReadUInt16(reader);
            additionalEntryCount = BinaryHelper.ReadUInt16(reader);
            for (int i = 0; i < questionEntryCount; i++)
                m.Questions.Add(Question.Get(reader));
            for (int i = 0; i < answerEntryCount; i++)
                m.Answers.Add(Answer.Get(reader));
            for (int i = 0; i < authorityEntryCount; i++)
                m.Authorities.Add(Answer.Get(reader));
            for (int i = 0; i < additionalEntryCount; i++)
                m.Additionals.Add(Answer.Get(reader));
            return m;
        }
        #endregion

        public Message Clone()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WriteTo(ms);
                ms.Position = 0;
                return GetMessage(new BinaryReader(ms));
            }
        }
    }
}
