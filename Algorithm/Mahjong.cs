using MahjongAlgorithm.Algorithm;
using MahjongAlgorithm.Extension;
using System;
using System.Collections.Generic;

namespace MahjongAlgorithm {
    
    public class Mahjong {
        
        static Mahjong() {
        }

        public static IAlgorithm GetAlgorithm(
            string tingTableFile = null,
            string laiTableFile = null,
            string huTableFile = null) {

            if (string.IsNullOrEmpty(tingTableFile) && string.IsNullOrEmpty(huTableFile) && string.IsNullOrEmpty(laiTableFile)) {
                return new DFSAlgorithm();
            }
            return new TableAlgorithm(tingTableFile,laiTableFile,huTableFile);
        }
    }
}