
private Node currentNode;
private Semaphore networkNodeLock = new Semaphore(1, 1);
private List<Node> networkNodes = new List<Node>();
private Queue<Node> newNodes = new Queue<Node>();
private Timer newNodeTimer;
private Timer pingTimer;
private Timer connectionCheckTimer;
private Semaphore clientRequestHandlerLock = new Semaphore(1, 1);
private List<ClientRequestHandler> clientRequestHandlers = new List<ClientRequestHandler>();
private CoreLogger logger;

HashTableWrapper hashTableWrapper = new HashTableWrapper();
public HashTableManager(Node CurrentNode, List<Node> NetworkNodes, HashTableWrapper hashTableImplementation, CoreLogger Logger)
{
	currentNode = CurrentNode;
	networkNodes = NetworkNodes;
	hashTableWrapper = hashTableImplementation;
	logger = Logger;
}

public Boolean Run()
{
	pingTimer = new Timer(pingTask, this, 1000 * 60, 1000 * 60);
	connectionCheckTimer = new Timer(connectionCheckTask, this, 1000 * 60, 1000 * 60);
	newNodeTimer = new Timer(newNodeTransferTask, this, 1000 * 60, 1000 * 60);
	return true;
}