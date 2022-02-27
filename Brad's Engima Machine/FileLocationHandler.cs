using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Brad_s_enigma_Machine
{
    static class FileLocationHandler
    {
        static private string mainDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        static private string settingsPath = @"\settings\";
        static private string masterConfigPath = mainDirectory + settingsPath;

        static private string enigma = @"enigma\";
        static private string cracking = @"cracking\";
        // --------------------
        static public readonly string logFileLocation_R     = masterConfigPath + enigma     + @"log files\";
        static public readonly string machineData_R         = masterConfigPath + enigma     + @"machine data\";
        static public readonly string cogs_R                = masterConfigPath + enigma     + @"cogs\";
        static public readonly string reversers_R           = masterConfigPath + enigma     + @"ukws\";
        static public readonly string messages_R            = masterConfigPath + enigma     + @"messages\";
        static public readonly string switchboard_R         = masterConfigPath + enigma     + @"switchboard\";
        static public readonly string readout_R             = masterConfigPath + enigma     + @"readout\";

        // ---------------------

        static public readonly string unigramFrequencies_R  = masterConfigPath + cracking   + @"ngram analysis\Old\unigram frequencies.txt"; // Faster but less accurate
        static public readonly string bigramFrequencies_R   = masterConfigPath + cracking   + @"ngram analysis\Old\bigram frequencies.txt";
        static public readonly string trigramFrequencies_R  = masterConfigPath + cracking   + @"ngram analysis\Old\trigram frequencies.txt";
        static public readonly string quadgramFrequencies_R = masterConfigPath + cracking   + @"ngram analysis\Old\quadgram frequencies.txt";
        static public readonly string commonWords_R         = masterConfigPath + cracking   + @"common words.txt";

        static public readonly string knownEnglishWords_R   = masterConfigPath + cracking   + @"testfiles\known english text.txt";
        static public readonly string bible30chapters_R     = masterConfigPath + cracking   + @"testfiles\bible30chapters.txt";
        static public readonly string bible30chaptersRand_R = masterConfigPath + cracking   + @"testfiles\bible30chaptersEnigma.txt";

        static public readonly string newBiGram_R           = masterConfigPath + cracking   + @"ngram analysis\Accurate_4000\newBiGram5000.txt";
        static public readonly string newTriGram_R          = masterConfigPath + cracking   + @"ngram analysis\Accurate_4000\newTriGram5000.txt";
        static public readonly string newQuadGram_R         = masterConfigPath + cracking   + @"ngram analysis\Accurate_4000\newQuadGram5000.txt";
        static public readonly string newQuintGram_R        = masterConfigPath + cracking   + @"ngram analysis\Accurate_4000\newQuintGram5000.txt";

        static public readonly string MSF_R                 = masterConfigPath + @"machine specific files\"; // Quick Refrence for msf locations

    }
}
