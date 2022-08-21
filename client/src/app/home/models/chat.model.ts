export interface ChatModel {
  id?: string;
  senderId: string;
  receiverId: string;
  messageText?: string;
  senderImage?: string;
  datetime?: Date;
}
