open System.Data.SqlClient

let rewriteSqlStatementToEnableMoreThanOneParameterDeclaration() =
        
    let parser = Microsoft.SqlServer.TransactSql.ScriptDom.TSql140Parser(true)
    let tsqlReader = new System.IO.StringReader("")
    let errors = ref Unchecked.defaultof<_>
    let fragment = parser.Parse(tsqlReader, errors)

    let allVars = ResizeArray()
    let declaredVars = ResizeArray()

    fragment.Accept {
        new Microsoft.SqlServer.TransactSql.ScriptDom.TSqlFragmentVisitor() with
            member __.Visit(node : Microsoft.SqlServer.TransactSql.ScriptDom.VariableReference) = 
                base.Visit node
                allVars.Add(node.Name, node.StartOffset, node.FragmentLength)
            member __.Visit(node : Microsoft.SqlServer.TransactSql.ScriptDom.DeclareVariableElement) = 
                base.Visit node
                declaredVars.Add(node.VariableName.Value)
    }
    let unboundVars = 
        allVars 
        |> Seq.groupBy (fun (name, _, _)  -> name)
        |> Seq.choose (fun (name, xs) -> 
            if declaredVars.Contains name 
            then None 
            else Some(name, xs |> Seq.mapi (fun i (_, start, length) -> sprintf "%s%i" name i, start, length)) 
        )
        |> dict

    unboundVars, !errors

[<EntryPoint>]
let main _ = 
    

    0