export interface chatModel {
  id?: string;
  senderId: string;
  receiverId: string;
  messageText?: string;
  senderImage?: string;
  datetime: Date;
}
