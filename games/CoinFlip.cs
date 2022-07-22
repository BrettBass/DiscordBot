public class CoinFlip {
    public static boolean flipCoin(int numFlips) {
        int numHeads = 0;
        for(int i = 0; i < numFlips; i++){
            if(Math.random() < 0.5)
                numHeads++;
        }
        return numHeads > numFlips / 2;
    }
    public static boolean flipCoin() {
        return flipCoin(1);
    }
}