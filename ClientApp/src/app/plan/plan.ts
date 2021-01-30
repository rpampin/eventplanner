import { PlanComponent } from './plan.component';

export class Plan {
    id: string;
    title: string;
    parts: PlanPart[] = [];

    constructor() { }
}

export class PlanPart {
    id: string;
    index: number;
    title: string;
    steps: PlanStep[] = [];

    constructor() { }
}

export class PlanStep {
    id: string;
    index: number;
    title: string;
    description: string;

    constructor() { }
}