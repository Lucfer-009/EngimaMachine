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
    { // Used to control file access across the program and ensure their relative mainDirectory.
        static private string mainDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        static private string settingsPath = @"\settings\";
        static private string masterConfigPath = mainDirectory + settingsPath;

        static private string enigma = @"enigma\";
        static private string cracking = @"cracking\";
        // --------------------
        static public readonly string logFileLocation_R         = masterConfigPath + enigma     + @"log files\";
        static public readonly string machineData_R             = masterConfigPath + enigma     + @"machine data\";
        static public readonly string cogs_R                    = masterConfigPath + enigma     + @"cogs\";
        static public readonly string reversers_R               = masterConfigPath + enigma     + @"ukws\";
        static public readonly string messages_R                = masterConfigPath + enigma     + @"messages\";
        static public readonly string switchboard_R             = masterConfigPath + enigma     + @"switchboard\";
        static public readonly string readout_R                 = masterConfigPath + enigma     + @"readout\";
        static public readonly string MSF_R                     = masterConfigPath + null       + @"machine specific files\"; // Quick Refrence for msf locations

        // ---------------------
        static public readonly string cyphertextMessages_R      = masterConfigPath + cracking   + @"messages\";

        static public readonly string unigramFrequencies_R      = masterConfigPath + cracking   + @"ngram analysis\unigram frequencies.txt"; 

        static public readonly string accurateBiGram_R          = masterConfigPath + cracking   + @"ngram analysis\Accurate\bi_500.txt";
        static public readonly string accurateTriGram_R         = masterConfigPath + cracking   + @"ngram analysis\Accurate\tri_1000.txt";
        static public readonly string accurateQuadGram_R        = masterConfigPath + cracking   + @"ngram analysis\Accurate\quad_2000.txt";
        static public readonly string accurateQuintGram_R       = masterConfigPath + cracking   + @"ngram analysis\Accurate\quint_4000.txt";

        static public readonly string fastBiGram_R              = masterConfigPath + cracking   + @"ngram analysis\Fast\bi_64.txt";
        static public readonly string fastTriGram_R             = masterConfigPath + cracking   + @"ngram analysis\Fast\tri_128.txt";
        static public readonly string fastQuadGram_R            = masterConfigPath + cracking   + @"ngram analysis\Fast\quad_256.txt";
        static public readonly string fastQuintGram_R           = masterConfigPath + cracking   + @"ngram analysis\Fast\quint_512.txt";

        static public readonly string foreword                  = masterConfigPath + @"foreword.txt";

    }
}
