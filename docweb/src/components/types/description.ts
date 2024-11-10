export interface Param {
  name: string;
  description: string;
}

export interface FunctionDescription {
  name: string;
  description: string;
  params?: Param[];
  returns?: string;
}

export interface ClassDescription {
  name: string;
  description: string;
  methods?: FunctionDescription[];
}

export interface FileDescription {
  name: string;
  overview?: string;
  functions?: FunctionDescription[];
  classes?: ClassDescription[];
  imports?: string[];
}

export interface RepoDescription {
  overview?: string;
  files: FileDescription[];
}
