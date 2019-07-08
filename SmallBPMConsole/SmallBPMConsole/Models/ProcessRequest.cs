using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SmallBPMConsole.Masstransit;

namespace SmallBPMConsole.Models
{
    public class ProcessContext
    {
        [Required]
        public string ProcessId { get; set; }
        public List<ProcessVariableData> Variables { get; set; }

        public ProcessConfig ProcessConfig { get; set; }

        public SequenceFlow CurrentSequenceFlow { get; set; }
    }
}
