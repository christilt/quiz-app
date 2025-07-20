using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DatabaseSource.Entities;
public class Quiz
{
    public int Id { get; private set; }
    public Guid Guid { get; private set; }
}
