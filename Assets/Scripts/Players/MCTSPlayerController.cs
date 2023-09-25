public class MCTSPlayerController : APlayerController
{
    private const int numberOfTests = 4;
    private const int numberOfSimulations = 8;
    private MCTSNode startNode;

    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        PlayerUpdateResult res = new PlayerUpdateResult();
        res.Position = Position;
        
        for (int i = 0; i < numberOfTests; i++)
        {
            MCTSNode selectedNode = Selection();
            MCTSNode newNode = Expand(selectedNode);
            int simulationScore = Simulate(newNode);
            BackPropagation(newNode,simulationScore,numberOfSimulations);
            Play();
        }

        return res;
    }

    MCTSNode Selection()
    {
        MCTSNode selectedNode;

        return selectedNode;
    }

    MCTSNode Expand(MCTSNode node)
    {
        return node;
    }

    int Simulate(MCTSNode node)
    {
        int numberWin = 0;
        
        for (int i = 0; i < numberOfSimulations; i++)
        {

            numberWin++;
        }
        
        return numberWin;
    }

    void BackPropagation(MCTSNode newNode, int numberVictory, int numberSimulation)
    {
        // ???
    }

    void Play()
    {
        
    }
}

public struct MCTSNode
{
    
}