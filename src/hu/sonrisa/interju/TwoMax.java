package hu.sonrisa.interju;

public class TwoMax {
    
    // Írasd ki a paraméterben megkapott integereket tartalmazó tömbből a két legnagyobb számot.
    public void PrintTwoLargestNumber(int[] numbers) {

    }

    public static void main(String[] args) {
        TwoMax twomax = new TwoMax();
        
        twomax.PrintTwoLargestNumber(new int[]{1,4,8,9});
        twomax.PrintTwoLargestNumber(new int[]{-5,30,1,2,3,15,10,20});
        twomax.PrintTwoLargestNumber(new int[]{5,-30,-1,-2,-3,-15,-10,-20});
    }
}
