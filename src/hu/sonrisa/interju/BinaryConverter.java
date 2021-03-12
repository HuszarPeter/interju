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
        if (number == null)
            return "";
        String text = "";
        int value = Math.abs(number % 2);
        text = Integer.toString(value) + text;
        int rest =  Math.abs(number / 2);
        while (rest > 0) {
            value = rest % 2;
            text = Integer.toString(value) + text;
            rest =  Math.abs(rest / 2);
        }
        while (text.length() < 7) {
            text = "0" + text;
        }
        if (number < 0) {
            text = "1" + text;
        }else if (number >= 0) {
            text = "0" + text;
        }
        return text;
    }

    public static void main(String[] args) {
        BinaryConverter converter = new BinaryConverter();

        for(byte num = -10; num < 127; num++) {
            System.out.println(converter.ConvertToBinaryString(num));
        }
    }
}
