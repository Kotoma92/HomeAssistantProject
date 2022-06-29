using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SpeechCommands;

[Table("SpeechModel")]
public class SpeechModel
{
    [Key]
    public string CommandId { get; set; }
    public DateTime DateTimeNow { get; set; }
    public string Commands { get; set; }

    public List<SpeechModel> GetAllFromDb()
    {
        return null;
    }
}

public class SpeechModelContext : DbContext
{
    public SpeechModelContext() : base("SpeechConnectionString")
    {
    }
    public DbSet<SpeechModel> Speech { get; set; }
}