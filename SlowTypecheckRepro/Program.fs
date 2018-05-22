open Microsoft.SqlServer.TransactSql.ScriptDom

let f() =
    let tsqlReader = new System.IO.StringReader("")
    let errors = ref Unchecked.defaultof<_>
    let fragment = TSql140Parser(true).Parse(tsqlReader, errors)
    
    fragment.Accept {
        new TSqlFragmentVisitor() with
            member __.Visit(node : VariableReference) = base.Visit node
            member __.Visit(node : DeclareVariableElement) = base.Visit node
    }

[<EntryPoint>]
let main _ = 
    

    0