using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Text;
using System.Reflection;
//using System.Windows.Forms;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
    /// 动态编译并执行方法的工具类
	/// </summary>
	public class Evaluator
	{
    #region Construction
	public Evaluator(EvaluatorItem[] items)
	{
		ConstructEvaluator(items);
	}

    public Evaluator(Type returnType, string expression, string name)
    {
      EvaluatorItem[] items = { new EvaluatorItem(returnType, expression, name) };
      ConstructEvaluator(items);
    }

    public Evaluator(EvaluatorItem item)
    {
      EvaluatorItem[] items = { item };
      ConstructEvaluator(items);
    }

    private void ConstructEvaluator(EvaluatorItem[] items)
    {
      ICodeCompiler comp = (new CSharpCodeProvider().CreateCompiler());
      CompilerParameters cp = new CompilerParameters();
      cp.ReferencedAssemblies.Add("system.dll");
      //cp.ReferencedAssemblies.Add("system.data.dll");
     // cp.ReferencedAssemblies.Add("system.xml.dll");
      cp.GenerateExecutable = false;
      cp.GenerateInMemory = true;

      StringBuilder code = new StringBuilder();
     code.Append("using System; \n");
     // code.Append("using System.Data; \n");
     // code.Append("using System.Data.SqlClient; \n");
     // code.Append("using System.Data.OleDb; \n");
    //  code.Append("using System.Xml; \n");
      code.Append("namespace HRMS { \n");
      code.Append("  public class _Evaluator { \n");
      foreach(EvaluatorItem item in items)
      {
        code.AppendFormat("    public {0} {1}() ", 
                          item.ReturnType.Name, 
                          item.Name);
        code.Append("{ ");
        //code.AppendFormat("      return ({0}); ", item.Expression);
	    code.AppendFormat("{0}", item.Expression);
        code.Append("}\n");
      }
      code.Append(" }\n}");
	  
//	  string sCode = code.ToString();
//	  MessageBox.Show(sCode);

		try
		{
			CompilerResults cr = comp.CompileAssemblyFromSource(cp, code.ToString());
			if (cr.Errors.HasErrors)
			{
				StringBuilder error = new StringBuilder();
				//error.Append("Error Compiling Expression: ");
				foreach (CompilerError err in cr.Errors)
				{
				error.AppendFormat("{0}", err.ErrorText);
				}
				throw new Exception(error.ToString());
			}
			Assembly a = cr.CompiledAssembly;
			_Compiled = a.CreateInstance("HRMS._Evaluator");
			sEvaluateResult="Success"; //Calculate success
		}
		catch(Exception e)
		{

			//MessageBox.Show(e.Message);
			_Compiled=null;
			sEvaluateResult=e.Message ;
		}
		finally
		{

		}

    }
    #endregion

    #region Public Members
        /// <summary>
    ///  执行动态编译代码中的方法（非静态），返回int
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
    public int EvaluateInt(string name)
    {
		if (sEvaluateResult.Equals("Success"))
           return (int) Evaluate(name);
		else
		   return (-1); //公式出错返回值
    }

        /// <summary>
    ///  执行动态编译代码中的方法（非静态），返回string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
    public string EvaluateString(string name)
    {
      if (sEvaluateResult.Equals("Success"))
		  return (string) Evaluate(name);
	  else
		  return ("");
    }

        /// <summary>
    ///  执行动态编译代码中的方法（非静态），返回bool
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
    public bool EvaluateBool(string name)
    {
      if (sEvaluateResult.Equals("Success"))
		  return (bool) Evaluate(name);
	  else
		  return(false);
    }

        /// <summary>
    ///  执行动态编译代码中的方法（非静态），返回object
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
    public object Evaluate(string name)
    {
		if (sEvaluateResult.Equals("Success"))
		{
			MethodInfo mi = _Compiled.GetType().GetMethod(name);
			return mi.Invoke(_Compiled, null);		
		}
		else
		{
			return(null);
		}		
    }
    #endregion

    #region Static Members
        /// <summary>
    ///  执行动态编译代码中的方法（静态），返回int
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
    static public int EvaluateToInteger(string code)
    {
	  Evaluator eval = new Evaluator(typeof(int), code, staticMethodName);
      if (sEvaluateResult.Equals("Success"))
		return (int) eval.Evaluate(staticMethodName);
	  else
	    return (-1);
    }

        /// <summary>
    /// 执行动态编译代码中的方法（静态），返回string
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
    static public string EvaluateToString(string code)
    {
      Evaluator eval = new Evaluator(typeof(string), code, staticMethodName);
      if (sEvaluateResult.Equals("Success"))
		return (string) eval.Evaluate(staticMethodName);
	  else
	    return ("");
    }
    
        /// <summary>
    /// 执行动态编译代码中的方法（静态），返回bool
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
    static public bool EvaluateToBool(string code)
    {
      Evaluator eval = new Evaluator(typeof(bool), code, staticMethodName);
      if (sEvaluateResult.Equals("Success"))
		return (bool) eval.Evaluate(staticMethodName);
	  else
	    return(false);
    }

        /// <summary>
    /// 执行动态编译代码中的方法（静态），返回object
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
    static public object EvaluateToObject(string code)
    {
      Evaluator eval = new Evaluator(typeof(object), code, staticMethodName);
      if (sEvaluateResult.Equals("Success"))
		  return eval.Evaluate(staticMethodName);
	  else
	     return (null);
    }

    #endregion

    #region Private
    const string staticMethodName = "__foo";
   // Type _CompiledType = null;
    object _Compiled = null;
	static public string   sEvaluateResult =""; //用于表示计算成功与否:bEvaluateSuccess=true计算成功,否则失败

    #endregion

	}

  public class EvaluatorItem
  {
    public Type ReturnType;
    public string Name;
    public string Expression;

    public EvaluatorItem(Type returnType, string expression, string name)
    {
      ReturnType = returnType;
      Expression = expression;
      Name = name;
    }

  }
}
