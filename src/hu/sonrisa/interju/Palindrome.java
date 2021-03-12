package hu.sonrisa.interju;

public class Palindrome {
    
    // A feladat, hogy a bejövő paraméterről el kell dönteni, hogy az plaindróma-e vagy sem
    // A palindróma olyan karakter sorozat amely balrol jobbra olvasva is pontosan ugyanaz mint jobbrol balra olvasva.
    public boolean IsPalindrome(String str) {
        return false;
    }


    public static void main(String[] args) {
        Palindrome palindrome = new Palindrome();

        System.out.println(palindrome.IsPalindrome("apa"));
        System.out.println(palindrome.IsPalindrome("indulagorogaludni"));
    }
}
