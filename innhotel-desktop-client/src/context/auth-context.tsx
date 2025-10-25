import { createContext } from "react";
import type { AuthContextType } from "@/types/api/auth";

export const AuthContext = createContext<AuthContextType | undefined>(undefined);
