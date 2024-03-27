using Google.OrTools.Sat;

public class SolutionPrinter : CpSolverSolutionCallback
{
    private int SolutionCount_;
    private BoolVar A_;
    private BoolVar B_;
    private BoolVar[] Oprs_;

    public SolutionPrinter(BoolVar a, BoolVar b, BoolVar[] oprs)
    {
        SolutionCount_ = 0;
        A_ = a;
        B_ = b;
        Oprs_ = oprs;
    }

    public override void OnSolutionCallback()
    {
        SolutionCount_++;
        Console.WriteLine(String.Format("Solution #{0}: time = {1:F5} s", SolutionCount_, WallTime()));
        Console.WriteLine($"  A = {Value(A_)}, B = {Value(B_)}");
        foreach (BoolVar opr in Oprs_)
        {
            Console.Write($"  {opr.ToString()} = {Value(opr)}");
        }
        Console.WriteLine();
    }

    public int SolutionCount()
    {
        return SolutionCount_;
    }
}

public class Problem
{
    static BoolVar BoolVarAnd(CpModel model, ILiteral a, ILiteral b, string? name = null)
    {
        if (name is null)
        {
            name = String.Format("({0})AND({1})", a.ToString(), b.ToString());
        }

        BoolVar c = model.NewBoolVar(name);

        model.Add((LinearExpr)a + (LinearExpr)b <= c + 1); // a = 1 AND b = 1 => c = 1
        model.Add(2 * c <= (LinearExpr)a + (LinearExpr)b); // c = 1 => a = 1 AND b = 1

        return c;
    }

    static BoolVar BoolVarOr(CpModel model, ILiteral a, ILiteral b, string? name = null)
    {
        if (name is null)
        {
            name = String.Format("({0})OR({1})", a.ToString(), b.ToString());
        }

        BoolVar c = model.NewBoolVar(name);

        model.Add((LinearExpr)a + (LinearExpr)b <= 2 * c); // a = 1 OR b = 1 => c = 1
        model.Add(c <= (LinearExpr)a + (LinearExpr)b); // c = 1 => a = 1 OR b = 1

        return c;
    }

    static BoolVar BoolVarNand(CpModel model, ILiteral a, ILiteral b, string? name = null)
    {
        if (name is null)
        {
            name = String.Format("({0})NAND({1})", a.ToString(), b.ToString());
        }

        BoolVar c = model.NewBoolVar(name);

        model.Add((LinearExpr)a + (LinearExpr)b <= c.NotAsExpr() + 1); // a = 1 AND b = 1 => c = 0
        model.Add(2 * c.NotAsExpr() <= (LinearExpr)a + (LinearExpr)b); // c = 0 => a = 1 AND b = 1

        return c;
    }
	
    static BoolVar BoolVarNor(CpModel model, ILiteral a, ILiteral b, string? name = null)
    {
        if (name is null)
        {
            name = String.Format("({0})NOR({1})", a.ToString(), b.ToString());
        }

        BoolVar c = model.NewBoolVar(name);

        model.Add((LinearExpr)a + (LinearExpr)b <= 2 * c.NotAsExpr()); // a = 1 OR b = 1 => c = 0
        model.Add(c.NotAsExpr() <= (LinearExpr)a + (LinearExpr)b); // c = 0 => a = 1 OR b = 1

        return c;
    }

    static BoolVar BoolVarXor(CpModel model, ILiteral a, ILiteral b, string? name = null)
    {
        if (name is null)
        {
            name = String.Format("({0})XOR({1})", a.ToString(), b.ToString());
        }

        BoolVar c = model.NewBoolVar(name);

        model.Add(a.NotAsExpr() + b.NotAsExpr() <= 1 + c.NotAsExpr()); // a = 0 AND b = 0 => c = 0
        model.Add((LinearExpr)a + (LinearExpr)b <= 1 + c.NotAsExpr()); // a = 1 AND b = 1 => c = 0
        model.Add((LinearExpr)a + b.NotAsExpr() <= 1 + c); // a = 1 AND b = 0 => c = 1
        model.Add(a.NotAsExpr() + (LinearExpr)b <= 1 + c); // a = 0 AND b = 1 => c = 1

        return c;
    }
	
    static BoolVar BoolVarXnor(CpModel model, ILiteral a, ILiteral b, string? name = null)
    {
        if (name is null)
        {
            name = String.Format("({0})XNOR({1})", a.ToString(), b.ToString());
        }

        BoolVar c = model.NewBoolVar(name);

        model.Add(a.NotAsExpr() + b.NotAsExpr() <= 1 + c); // a = 0 AND b = 0 => c = 1
        model.Add((LinearExpr)a + (LinearExpr)b <= 1 + c); // a = 1 AND b = 1 => c = 1
        model.Add((LinearExpr)a + b.NotAsExpr() <= 1 + c.NotAsExpr()); // a = 1 AND b = 0 => c = 0
        model.Add(a.NotAsExpr() + (LinearExpr)b <= 1 + c.NotAsExpr()); // a = 0 AND b = 1 => c = 0

        return c;
    }
    
    static void Main()
    {
        CpModel model = new();

        BoolVar a = model.NewBoolVar("A");
        BoolVar b = model.NewBoolVar("B");
        BoolVar aANDb = BoolVarAnd(model, a, b);
        BoolVar aORb = BoolVarOr(model, a, b);
        BoolVar aNANDb = BoolVarNand(model, a, b);
        BoolVar aNORb = BoolVarNor(model, a, b);
        BoolVar aXORb = BoolVarXor(model, a, b);
        BoolVar aXNORb = BoolVarXnor(model, a, b);

        CpSolver solver = new();
        SolutionPrinter printer = new(a, b, new BoolVar[] { aANDb, aORb, aNANDb, aNORb, aXORb, aXNORb });
        solver.StringParameters = "enumerate_all_solutions:true";
        solver.Solve(model, printer);

        Console.WriteLine($"Number of solutions found: {printer.SolutionCount()}");
    }
}
