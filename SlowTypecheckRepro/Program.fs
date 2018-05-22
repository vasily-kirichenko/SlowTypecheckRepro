open Microsoft.SqlServer.TransactSql.ScriptDom

type T() =
    inherit TSqlFragmentVisitor()
    override __.Visit(node : VariableReference) = base.Visit node
    override __.Visit(node : DeclareVariableElement) = base.Visit node

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
    L

    0