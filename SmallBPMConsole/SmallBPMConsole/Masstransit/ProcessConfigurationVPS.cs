using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SmallBPMConsole.Masstransit
{
    [XmlRoot("process")]
    public class ProcessConfig
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("status")]
        public string Status { get; set; }
        [XmlAttribute("application")]
        public string Application { get; set; }

        [XmlElement("rabbitMqUri")]
        public RabbitMqUri RabbitMqUri { get; set; }
        [XmlElement("startEvent")]
        public StartEvent StartEvent { get; set; }
        [XmlElement("endEvent")]
        public EndEvent EndEvent { get; set; }
        [XmlElement("userTask")]
        public List<UserTask> UserTask { get; set; }
        [XmlElement("exclusiveGateway")]
        public List<ExclusiveGateway> ExclusiveGateway { get; set; }
        [XmlElement("sequenceFlow")]
        public List<SequenceFlow> SequenceFlow { get; set; }
    }
    [XmlType("rabbitMqUri")]
    public class RabbitMqUri
    {
        [XmlAttribute("url")]
        public string Url { get; set; }
        [XmlAttribute("user")]
        public string User { get; set; }
        [XmlAttribute("password")]
        public string Password { get; set; }
    }
    [XmlType("startEvent")]
    public class StartEvent
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }
    [XmlType("endEvent")]
    public class EndEvent
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }
    [XmlType("userTask")]
    public class UserTask
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
    [XmlType("exclusiveGateway")]
    public class ExclusiveGateway
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }
    [XmlType("sequenceFlow")]
    public class SequenceFlow
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("sourceRef")]
        public string SourceRef { get; set; }
        [XmlAttribute("targetRef")]
        public string TargetRef { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("conditionExpression")]
        public ConditionExpression ConditionExpression { get; set; }
    }

    [XmlType("conditionExpression")]
    public class ConditionExpression
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }

}
