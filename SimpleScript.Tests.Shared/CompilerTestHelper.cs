using FluentAssertions;
using System.Text.RegularExpressions;

namespace SimpleScript.Tests.Shared
{
    public static class CompilerTestHelper
    {
        public static string ConvertToCCode(string expectedBody, string? functionDeclaration = null)
        {
            return @$"
                #include <stdio.h>
                #include <string.h>
                #include <stdlib.h>
                typedef struct Node {{
                    void *data;
                    struct Node *next;
                }} Node;
                Node *head = NULL;
                void add_to_list(void *data) {{
                    Node *new_node = (Node *)malloc(sizeof(Node));
                    new_node->data = data;
                    new_node->next = head;
                    head = new_node;
                }}
                void free_list() {{
                    Node *current = head;
                    Node *next_node;
                    while (current != NULL) {{
                        next_node = current->next;
                        free(current->data);
                        free(current);
                        current = next_node;
                    }}
                    head = NULL;
                }}
                {functionDeclaration}
                int main() {{
                    {expectedBody}
                    return 0;
                }}
            ";
        }

        public static string ConvertToCCode(string[] expectedBody, string[] functionDeclarations)
        {
            return ConvertToCCode(string.Join("\n", expectedBody), string.Join("\n", functionDeclarations));
        }

        public static string NormalizeWhiteSpace(string input)
        {
            return Regex.Replace(input, @"\s+", " ").Trim();
        }

        public static void AssertNormalizedStrings(string actual, string expected)
        {
            NormalizeWhiteSpace(actual).Should().Be(NormalizeWhiteSpace(expected));
        }
    }
}
