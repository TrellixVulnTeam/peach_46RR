; This header file consolidate definitions for Windows
; running on both ia32 and intel64 architecture.
; Note that all definitions here are according to the 'cdecl'
; calling convention (the default in Windows)

;-------------------------------------------------------------------
; Architecture specific macros:
;-------------------------------------------------------------------
; PROLOGUE                  - The prologue of the program - must be placed before
;                             any other declaration
; BEGIN_STACK_FRAME         - Expands to the instructions that build a new stack
;                             frame for a function
; END_STACK_FRAME           - Expands to the instructions that destroy a stack
;                             frame just before calling "ret"
; PARAM1                    - The first argument to a function.
;                             Note that this macro is valid only after building a
;                             stack frame
; PARAM2                    - The second argument to a function.
;                             Note that this macro is valid only after building a
;                             stack frame
; RETURN_REG                - The register that holds the return value
; GDI_REG                   - edi/rdi depending on the architecture
; GSI_REG                   - esi/rsi depending on the architecture
; NATIVE_SIZE_SUFFIX <inst> - Add the suffix representing the word size native to
;                             the current target to instruction <inst>.
;                             For example "NATIVE_SIZE_SUFFIX popf" will be expanded
;                             to popfd on ia32, and popfq on intel64.
; STACK_PTR                 - The stack pointer register
; ADDRINT_PTR               - Prefix for memory operands to refer to sizeof(ADDRINT)
; ADDRINT_TYPE              - Assembler type of ADDRINT
;-------------------------------------------------------------------

IFDEF TARGET_IA32
    PROLOGUE MACRO
        .686
        .XMM
        .model flat, c
    ENDM
    BEGIN_STACK_FRAME macro
        push ebp
        mov ebp,esp
    ENDM
    END_STACK_FRAME macro
        mov esp,ebp
        pop ebp
    ENDM
    PARAM1 textequ <[EBP+8]>
    PARAM2 textequ <[EBP+12]>
    RETURN_REG textequ <EAX>
    GDI_REG textequ <EDI>
    GSI_REG textequ <ESI>
	GAX_REG textequ <EAX>
	GCX_REG textequ <ECX>
	GDX_REG textequ <EDX>
	CL_REG  textequ <CL>
    STACK_PTR textequ <ESP>
    ADDRINT_PTR textequ <DWORD PTR>
    ADDRINT_TYPE textequ <DWORD>
	NATIVE_SIZE_SUFFIX macro inst
	    &inst&d
	ENDM
ELSE
    PROLOGUE MACRO
    ENDM
    BEGIN_STACK_FRAME macro
        push rbp
        mov rbp,rsp
    ENDM
    END_STACK_FRAME macro
        mov rsp,rbp
        pop rbp
    ENDM
    PARAM1 textequ <RCX>
    PARAM2 textequ <RDX>
    RETURN_REG textequ <RAX>
    GDI_REG textequ <RDI>
    GSI_REG textequ <RSI>
	GAX_REG textequ <RAX>
	GCX_REG textequ <RCX>
	GDX_REG textequ <RDX>
	CL_REG  textequ <CL>
    STACK_PTR textequ <RSP>
    ADDRINT_PTR textequ <QWORD PTR>
    ADDRINT_TYPE textequ <QWORD>
	NATIVE_SIZE_SUFFIX macro inst
	    &inst&q
	ENDM
ENDIF
