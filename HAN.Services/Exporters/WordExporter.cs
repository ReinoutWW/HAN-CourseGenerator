﻿using HAN.Services.Writers;

namespace HAN.Services.Exporters;

public class WordExporter() : FileExporter(new WordWriter())
{
    protected override void PrepareFile()
    {
        Console.WriteLine("Preparing Word file...");
    }

    protected override void FinalizeFile()
    {
        Console.WriteLine("Word export finalized.");
    }
}