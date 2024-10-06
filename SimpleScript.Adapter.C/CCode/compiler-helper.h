typedef struct Node
{
    void* data;
    struct Node* next;
} Node;

void add_to_list(void* data);

void free_list();
