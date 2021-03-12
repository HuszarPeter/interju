package hu.sonrisa.interju;

public class BinaryConverter {
    
    public String ConvertToBinaryString(Integer number) {
        return "";
    }


    public static void main(String[] args) {
        BinaryConverter converter = new BinaryConverter();

        for(int num = 1; num < 30; num++) {
            System.out.println(converter.ConvertToBinaryString(num));
        }
    }
}
