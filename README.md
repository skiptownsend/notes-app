# Notes App - Agentic Development Learning Project

A learning project to master multi-agent software development workflows using Claude Code CLI.

## Purpose

This project exists to learn and practice **agentic development** - using specialized AI agents to accelerate software delivery while maintaining engineering discipline and best practices.

### What I'm Learning

- **Multi-Agent Coordination**: How to orchestrate specialized AI agents (orchestrator, TDD agent, code reviewer, git workflow agent, etc.)
- **Test-Driven Development (TDD)**: Writing tests first, then implementing code to pass them
- **Git Workflow Best Practices**: Feature branches, conventional commits, pull requests, code review
- **Engineering Discipline with AI**: Using AI tools as force multipliers, not replacements for sound engineering judgment
- **Claude Code CLI**: Practical experience with agentic coding tools in a real development environment

### The Application

A simple notes management system:
- **Backend**: ASP.NET Core Web API (.NET 10) with in-memory storage
- **Frontend**: Angular 17+
- **Focus**: The tech stack is intentionally simple - the goal is learning the *process*, not building complex features

## Project Philosophy

This project demonstrates the distinction between "vibe coding" and **engineered agentic development**:

- ✅ AI agents accelerate delivery
- ✅ Engineers control the workflow
- ✅ Code is reviewed before merging
- ✅ Tests are written first (TDD)
- ✅ Documentation stays current
- ✅ Git history is clean and meaningful

## Agent System

This project uses specialized Claude Code CLI agents:

- **orchestrator** - Coordinates all other agents, never writes code directly
- **tdd-agent** - Writes tests first following TDD workflow
- **csharp-developer** - Implements C# backend code
- **angular-developer** - Implements Angular frontend code
- **code-reviewer** - Reviews code quality, security, and best practices
- **git-workflow-agent** - Manages branches, commits, and pull requests
- **documentation-agent** - Keeps documentation synchronized with code

## Development Environment

- **OS**: WSL2 (Ubuntu) on Windows 11
- **IDE**: VS Code with WSL extension
- **Version Control**: Git + GitHub
- **AI Tool**: Claude Code CLI with custom agents

## Setup

Prerequisites installed:
- .NET 10 SDK
- Node.js 20+ and npm
- Angular CLI
- Claude Code CLI
- Git + GitHub CLI

## What This Project Is NOT

This is **not** a production application. It's a learning sandbox for:
- Understanding agentic development workflows
- Practicing TDD discipline
- Learning Claude Code CLI capabilities
- Building skills in AI-assisted software engineering

## Status

🚧 **In Progress** - Currently setting up the multi-agent environment and learning the workflow.

## Learning Resources

Inspired by LinkedIn discussions on distinguishing between "vibe coding" and professional use of AI development tools by experienced engineers.

---

*"AI agents are not a replacement for engineering knowledge - they're an accelerator for engineers who understand SDLC, testing, code review, and architectural principles."*
