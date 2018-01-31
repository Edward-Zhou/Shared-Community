export interface Thread
{
    id: string,
    createdBy: string,
    createdOn: Date,
    forum: string,
    state: ThreadStatus,
    title: string,
    uri: string,
    lastActiveOn: Date
}

enum ThreadStatus
{
    Answered,
    Discussion,
    Proposed,
    Unanswered
}