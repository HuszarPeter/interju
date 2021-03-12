package hu.sonrisa.interju;

public class BinaryConverter {
    
    // A metódusnak át kell konvertálnia egy 10-es számrendszerbéli byte méretű számot a 2-es számrendszerbeli string-es reprezentációjára
    // úgy, hogy:
    // - A visszatérési érték első karaktere '1' legyen, ha az érték negatív
    // - A visszatérési érték első karaktere '0' legyen, ha az érték pozitív vagy 0
    // - Az eredmény mindenképpen 8 karakter hosszú legyen, tehát egy teljes byte-ot reprezentáljon, ha szükséges '0'-al kell balról feltölteni
    //   figyelembe vége az előjel bit értékét!
    // Pár példa:
    // 0  -> 00000000
    // 1  -> 00000001
    // -1 -> 10000001
    public String ConvertToBinaryString(Byte number) {
        return "";
    }

    public static void main(String[] args) {
        BinaryConverter converter = new BinaryConverter();

        for(byte num = -10; num < 127; num++) {
            System.out.println(converter.ConvertToBinaryString(num));
        }
    }
}
