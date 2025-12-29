using System;

namespace arrastre_archivos.exceptions;

public class PartidasNotFoundException : Exception
{
    public PartidasNotFoundException(string message) : base(message)
    {
    }
}
