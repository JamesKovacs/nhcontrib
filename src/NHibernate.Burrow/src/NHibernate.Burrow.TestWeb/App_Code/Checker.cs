using NHibernate.Burrow;

/// <summary>
/// Summary description for Checker
/// </summary>
public class Checker
{
    private static BurrowFramework f = new BurrowFramework();

    public static void CheckSpanningConversations(int numOfSpanning)
    {
        int conversations = f.BurrowEnvironment.SpanningConversations;
        if (conversations != numOfSpanning)
        {
            throw new AssertException("Expected spanning conversation " + numOfSpanning + " but is " + conversations);
        }
    }

    public static void AssertEqual(object expected, object val)
    {
        if (expected == null && val == null)
        {
            return;
        }
        if (expected != null && expected.Equals(val))
        {
            return;
        }

        throw new AssertException("Expected " + expected + " but was " + val);
    }
}