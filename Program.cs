//*****************************************************************************
//** 2054. Two Best Non-Overlapping Events    leetcode                       **
//*****************************************************************************

typedef struct {
    int endTime;
    int value;
} HeapNode;

typedef struct {
    HeapNode* data;
    int size;
    int capacity;
} MinHeap;

// Initialize a min-heap
MinHeap* createMinHeap(int capacity) {
    MinHeap* heap = (MinHeap*)malloc(sizeof(MinHeap));
    heap->data = (HeapNode*)malloc(capacity * sizeof(HeapNode));
    heap->size = 0;
    heap->capacity = capacity;
    return heap;
}

// Swap two heap nodes
void swap(HeapNode* a, HeapNode* b) {
    HeapNode temp = *a;
    *a = *b;
    *b = temp;
}

// Heapify up to maintain min-heap property
void heapifyUp(MinHeap* heap, int index) {
    while (index > 0) {
        int parent = (index - 1) / 2;
        if (heap->data[index].endTime >= heap->data[parent].endTime)
            break;
        swap(&heap->data[index], &heap->data[parent]);
        index = parent;
    }
}

// Heapify down to maintain min-heap property
void heapifyDown(MinHeap* heap, int index) {
    while (1) {
        int left = 2 * index + 1;
        int right = 2 * index + 2;
        int smallest = index;

        if (left < heap->size && heap->data[left].endTime < heap->data[smallest].endTime)
            smallest = left;
        if (right < heap->size && heap->data[right].endTime < heap->data[smallest].endTime)
            smallest = right;

        if (smallest == index)
            break;

        swap(&heap->data[index], &heap->data[smallest]);
        index = smallest;
    }
}

// Insert a new node into the heap
void insertHeap(MinHeap* heap, int endTime, int value) {
    heap->data[heap->size].endTime = endTime;
    heap->data[heap->size].value = value;
    heap->size++;
    heapifyUp(heap, heap->size - 1);
}

// Remove the top (minimum) node from the heap
HeapNode removeMin(MinHeap* heap) {
    HeapNode root = heap->data[0];
    heap->data[0] = heap->data[--heap->size];
    heapifyDown(heap, 0);
    return root;
}

// Check if the heap is empty
int isHeapEmpty(MinHeap* heap) {
    return heap->size == 0;
}

// Free the heap
void freeHeap(MinHeap* heap) {
    free(heap->data);
    free(heap);
}

// Comparison function for sorting events by start time
int compareEvents(const void* a, const void* b) {
    int* eventA = *(int**)a;
    int* eventB = *(int**)b;
    return eventA[0] - eventB[0];
}

int maxTwoEvents(int** events, int eventsSize, int* eventsColSize) {
    // Sort events by start time
    qsort(events, eventsSize, sizeof(int*), compareEvents);

    // Initialize the min-heap
    MinHeap* heap = createMinHeap(eventsSize);

    int result = 0;
    int best = 0;

    for (int i = 0; i < eventsSize; ++i) {
        // Remove events from the heap that end before the current event's start time
        while (!isHeapEmpty(heap) && heap->data[0].endTime < events[i][0]) {
            HeapNode top = removeMin(heap);
            if (top.value > best) {
                best = top.value;
            }
        }

        // Calculate the current result
        int currentSum = best + events[i][2];
        if (currentSum > result) {
            result = currentSum;
        }

        // Add the current event to the heap
        insertHeap(heap, events[i][1], events[i][2]);
    }

    // Free the heap
    freeHeap(heap);

    return result;
}