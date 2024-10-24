#include <stdio.h>
#include <string.h>
#include <stdlib.h>

typedef struct Node
{
    void* data;
    struct Node* next;
} Node;

Node* head = NULL;

void add_to_list(void* data)
{
    Node* new_node = (Node*)malloc(sizeof(Node));
    new_node->data = data;
    new_node->next = head;
    head = new_node;
}

void free_list()
{
    Node* current = head;
    while (current != NULL)
    {
        Node* next_node = current->next;
        free(current->data);
        free(current);
        current = next_node;
    }
    head = NULL;
}
